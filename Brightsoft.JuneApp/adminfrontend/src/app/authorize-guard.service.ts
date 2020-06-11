import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from "@angular/router";
import { JWTTokenService } from "./jwt-token.service";

@Injectable({
  providedIn: "root",
})
export class AuthorizeGuard implements CanActivate {
  constructor(private jwtService: JWTTokenService, private router: Router) {}
  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): any {
    if (this.jwtService.getUser() && !this.jwtService.isTokenExpired()) {
      return true;
    } else {
      this.router.navigate(["/login"]);
      return false;
    }
  }
}
