import { HttpRequest, HttpInterceptor, HttpHandler } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JWTTokenService } from './jwt-token.service';
@Injectable()
export class UniversalAppInterceptor implements HttpInterceptor {
 
  constructor( private jWTTokenService: JWTTokenService) { }
 
  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const token = this.jWTTokenService.jwtToken;
    req = req.clone({
      url:  req.url,
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next.handle(req);
  }
}