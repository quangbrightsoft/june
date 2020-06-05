import { UserModel } from "./../../../models/userModel";
import { UserService } from "./../../user.service";
import { Component } from "@angular/core";
import { DataSource } from "@angular/cdk/table";
import { Observable, BehaviorSubject } from "rxjs";
import { Router } from "@angular/router";

@Component({
  templateUrl: "user.component.html",
})
export class UserComponent {
  users: any[];
  loading = true;
  error: any;
  dataSource = new ExampleDataSource();
  displayedColumns: string[] = ["id", "userName", "email"];
  sortData = { column: "", desc: false };
  constructor(private userService: UserService, private router: Router) {}
  ngOnInit() {
    this.getUsers();
  }
  getUsers() {
    let params = {};
    if (this.sortData.column) {
      Object.assign(params, {
        descending: this.sortData.desc,
        sortBy: this.sortData.column,
      });
    }
    this.loading = true;
    this.userService.get(params).subscribe((result: any) => {
      if (!result.errors) {
        this.dataSource.data.next(result.data.users);
        this.loading = false;
      } else {
        console.error("ERROR in login", result.errors);
      }
    });
  }
  sort(column: string) {
    let descending =
      column === this.sortData.column ? !this.sortData.desc : false;
    this.sortData = { column: column, desc: descending };

    this.getUsers();
  }

  goTo(route: string) {
    this.router.navigate([route]).catch(console.log);
  }
}
export class ExampleDataSource extends DataSource<UserModel> {
  /** Stream of data that is provided to the table. */
  data = new BehaviorSubject<UserModel[]>([]);

  /** Connect function called by the table to retrieve one stream containing the data to render. */
  connect(): Observable<UserModel[]> {
    return this.data;
  }

  disconnect() {}
}
