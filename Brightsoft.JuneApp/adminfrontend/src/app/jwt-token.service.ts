import { Injectable } from "@angular/core";
import * as jwt_decode from "jwt-decode";
import { Router } from "@angular/router";

@Injectable({
  providedIn: "root",
})
export class JWTTokenService {
  decodedToken: { [key: string]: string };

  constructor(private router: Router) {}

  logout() {
    localStorage.removeItem("userToken");
    this.decodedToken = undefined;
    this.router.navigate(["/login"]);
  }
  get() {
    return localStorage.getItem("userToken");
  }

  setToken(token: string) {
    if (token) {
      localStorage.setItem("userToken", token);
    }
  }

  decodeToken() {
    if (localStorage.getItem("userToken")) {
      this.decodedToken = jwt_decode(localStorage.getItem("userToken"));
    }
  }

  getDecodeToken() {
    return jwt_decode(localStorage.getItem("userToken"));
  }

  getUser() {
    this.decodeToken();
    return this.decodedToken ? this.decodedToken.sub : null;
  }

  getEmailId() {
    this.decodeToken();
    return this.decodedToken ? this.decodedToken.email : null;
  }

  getRoles() {
    this.decodeToken();
    return this.decodedToken
      ? this.decodedToken[
          "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        ].split(",")
      : null;
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
