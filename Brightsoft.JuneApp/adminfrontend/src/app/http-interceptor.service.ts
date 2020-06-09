import {
  HttpRequest,
  HttpInterceptor,
  HttpHandler,
} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JWTTokenService } from "./jwt-token.service";
@Injectable()
export class UniversalAppInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    req = req.clone({
      url: req.url,
      setHeaders: {
        Authorization: `Bearer ${localStorage.getItem('userToken')}`,
      },
    });
    return next.handle(req);
  }
}
