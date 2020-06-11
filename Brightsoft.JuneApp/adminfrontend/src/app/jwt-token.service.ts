import { Injectable } from "@angular/core";
import * as jwt_decode from "jwt-decode";
import { Router } from "@angular/router";
import { Apollo } from "apollo-angular";
import gql from "graphql-tag";

@Injectable({
  providedIn: "root",
})
export class JWTTokenService {
  decodedToken: { [key: string]: string };
  refreshToken: string;

  constructor(private router: Router, private apollo: Apollo) {}

  getRefreshToken(params: any) {
    return this.apollo.mutate({
      mutation: gql`
        mutation RefreshToken(
          $authenticationToken: String!
          $refreshToken: String!
        ) {
          refreshToken(
            authenticationToken: $authenticationToken
            refreshToken: $refreshToken
          ) {
            accessToken
            refreshToken
          }
        }
      `,
      variables: params,
    });
  }
  logout() {
    localStorage.removeItem("userToken");
    this.decodedToken = undefined;
    this.router.navigate(["/login"]);
  }
  get() {
    return {
      authenticationToken: localStorage.getItem("userToken"),
      refreshToken: localStorage.getItem("refreshToken"),
    };
  }

  setToken(token: string, refreshToken: string) {
    localStorage.setItem("userToken", token);
    localStorage.setItem("refreshToken", refreshToken);
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
    var tokenRolesValue = this.decodedToken
      ? this.decodedToken[
          "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        ]
      : [];

    if (
      typeof tokenRolesValue == "object" &&
      <object>tokenRolesValue.constructor === Array
    ) {
      return tokenRolesValue;
    }
    return [tokenRolesValue];
  }

  getExpiryTime() {
    this.decodeToken();
    return this.decodedToken ? this.decodedToken.exp : null;
  }

  isTokenExpired(): boolean {
    return false;
    //TODO
    const expiryTime: number = parseInt(this.getExpiryTime());
    if (expiryTime) {
      return 1000 * expiryTime - new Date().getTime() < 0;
    } else {
      return false;
    }
  }
}
