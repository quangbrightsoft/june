import { Injectable } from "@angular/core";
import * as jwt_decode from "jwt-decode";
import { AppCookieService } from "./cookie.service";

@Injectable({
  providedIn: "root",
})
export class JWTTokenService {
  decodedToken: { [key: string]: string };

  constructor(private appCookieService: AppCookieService) {}

  get() {
    return this.appCookieService.get("userToken");
  }
  
  setToken(token: string) {
    if (token) {
      this.appCookieService.set("userToken", token);
    }
  }

  decodeToken() {
    if (this.appCookieService.get("userToken")) {
      this.decodedToken = jwt_decode(this.appCookieService.get("userToken"));
    }
  }

  getDecodeToken() {
    return jwt_decode(this.appCookieService.get("userToken"));
  }

  getUser() {
    this.decodeToken();
    return this.decodedToken ? this.decodedToken.sub : null;
  }

  getEmailId() {
    this.decodeToken();
    return this.decodedToken ? this.decodedToken.email : null;
  }

  getExpiryTime() {
    this.decodeToken();
    return this.decodedToken ? this.decodedToken.exp : null;
  }

  isTokenExpired(): boolean {
    const expiryTime: number = parseInt(this.getExpiryTime());
    if (expiryTime) {
      return 1000 * expiryTime - new Date().getTime() < 5000;
    } else {
      return false;
    }
  }
}
