import {
  HttpRequest,
  HttpInterceptor,
  HttpHandler,
} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JWTTokenService } from "./jwt-token.service";
import { catchError, filter, take, switchMap } from "rxjs/operators";
import { throwError, BehaviorSubject } from "rxjs";
@Injectable()
export class UniversalAppInterceptor implements HttpInterceptor {
  private refreshTokenInProgress = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(
    null
  );

  constructor(private jWTTokenService: JWTTokenService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    req = req.clone({
      url: req.url,
      setHeaders: {
        Authorization: `Bearer ${this.jWTTokenService.get().authenticationToken}`,
      },
    });
    return next.handle(req).pipe(
      catchError((err) => {
        // If error status is different than 401 we want to skip refresh token
        // So we check that and throw the error if it's the case
        if (err.status !== 401) {
          return throwError(err);
        }

        if (this.refreshTokenInProgress) {
          // If refreshTokenInProgress is true, we will wait until refreshTokenSubject has a non-null value
          // – which means the new token is ready and we can retry the request again
          return this.refreshTokenSubject.pipe(
            filter((result) => result !== null),
            take(1),
            switchMap(() => next.handle(this.addAuthenticationToken(req)))
          );
        } else {
          this.refreshTokenInProgress = true;

          // Set the refreshTokenSubject to null so that subsequent API calls will wait until the new token has been retrieved
          this.refreshTokenSubject.next(null);
          let tokens = this.jWTTokenService.get();
          // Call auth.refreshAccessToken(this is an Observable that will be returned)
          return this.jWTTokenService.getRefreshToken(tokens).pipe(
            switchMap((token: any) => {
              //When the call to refreshToken completes we reset the refreshTokenInProgress to false
              // for the next time the token needs to be refreshed
              this.refreshTokenInProgress = false;
              this.refreshTokenSubject.next(token);
              this.jWTTokenService.setToken(
                token.data.refreshToken.accessToken,
                token.data.refreshToken.refreshToken
              );
              return next.handle(this.addAuthenticationToken(req));
            }),
            catchError((err: any) => {
              this.refreshTokenInProgress = false;

              this.jWTTokenService.logout();
              return throwError(err);
            })
          );
        }
      })
    );
  }

  addAuthenticationToken(request: HttpRequest<any>) {
    // Get access token from Local Storage
    const accessToken = this.jWTTokenService.get().authenticationToken;

    // If access token is null this means that user is not logged in
    // And we return the original request
    if (!accessToken) {
      return request;
    }

    // We clone the request, because the original request is immutable
    return request.clone({
      setHeaders: {
        Authorization:
          "Bearer " + this.jWTTokenService.get().authenticationToken,
      },
    });
  }
}
