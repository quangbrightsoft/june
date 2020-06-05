import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from "@angular/router";
import { JWTTokenService } from "./jwt-token.service";
import { UserService } from "./user.service";

@Injectable({
  providedIn: "root",
})
export class AuthorizeGuard implements CanActivate {
  constructor(
    private userService: UserService,
    private jwtService: JWTTokenService,
    private router: Router
  ) {}
  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): any {
    if (this.jwtService.getUser()) {
      if (this.jwtService.isTokenExpired()) {
        // Should Redirect Sig-In Page
        this.router.navigate(["/login"]);
      } else {
        return true;
      }
    }else {
        this.router.navigate(["/login"]);
     
    }
  }
}
