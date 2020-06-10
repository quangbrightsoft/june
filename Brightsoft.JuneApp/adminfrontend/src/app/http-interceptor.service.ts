import {
  HttpRequest,
  HttpInterceptor,
  HttpHandler,
} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JWTTokenService } from "./jwt-token.service";
import { catchError } from "rxjs/operators";
import { throwError, Observable } from "rxjs";
@Injectable()
export class UniversalAppInterceptor implements HttpInterceptor {
  constructor(private jWTTokenService: JWTTokenService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    req = req.clone({
      url: req.url,
      setHeaders: {
        Authorization: `Bearer ${localStorage.getItem("userToken")}`,
      },
    });
    return next.handle(req).pipe(
      catchError((err) => {
        if ([401, 403].includes(err.status) && this.jWTTokenService.getUser()) {
          // auto logout if 401 or 403 response returned from api
          this.jWTTokenService.logout();
        }

        const error = (err && err.error && err.error.message) || err.statusText;
        console.error(err);
        return throwError(error);
      })
    );
  }
}
