import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { Apollo } from "apollo-angular";
import gql from "graphql-tag";

@Injectable({
  providedIn: "root",
})
export class UserService {
  constructor(private apollo: Apollo) {}

  get(params: any): Observable<any> {
    return this.apollo.mutate({
      mutation: gql`
        query QueryUsers($sortBy: String, $descending: Boolean) {
          users(sortBy: $sortBy, descending: $descending) {
            email
            createdAt
            deletedAt
            id
            isDeleted
            isDisabled
            roles
            ssn
            updatedAt
            userName
          }
        }
      `,
      variables: params,
    });
  }
  register(params: any): Observable<any> {
    return this.apollo.mutate({
      mutation: gql`
        mutation Register(
          $email: String!
          $username: String!
          $password: String!
        ) {
          register(email: $email, username: $username, password: $password) {
            accessToken
          }
        }
      `,
      variables: params,
    });
  }
  login(params: any): Observable<any> {
    return this.apollo.mutate({
      mutation: gql`
        mutation Login($id: String!, $password: String!) {
          login(username: $id, password: $password) {
            userName
            accessToken
          }
        }
      `,
      variables: params,
    });
  }
}
