import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { Apollo } from "apollo-angular";
import gql from "graphql-tag";

@Injectable({
  providedIn: "root",
})
export class UserService {
  constructor(private apollo: Apollo) {}

  getList(params: any): Observable<any> {
    let token = document.cookie;
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
  get(id: string): Observable<any> {
    let token = document.cookie;
    return this.apollo.mutate({
      mutation: gql`
        query QueryUsers($id: String) {
          user(id: $id) {
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
      variables: { id: id },
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
  create(params: any): Observable<any> {
    return this.apollo.mutate({
      mutation: gql`
        mutation Create(
          $fullName: String!
          $email: String!
          $roles: [String]!
        ) {
          createUser(fullName: $fullName, email: $email, roles: $roles) {
            id
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
