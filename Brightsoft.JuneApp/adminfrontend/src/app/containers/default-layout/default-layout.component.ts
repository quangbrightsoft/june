import { JWTTokenService } from "./../../jwt-token.service";
import { Component, OnInit } from "@angular/core";
import { AppSidebarService } from "./app-sidebar.service";

@Component({
  selector: "app-dashboard",
  templateUrl: "./default-layout.component.html",
})
export class DefaultLayoutComponent implements OnInit {
  public sidebarMinimized = false;
  constructor(
    public appSidebarService: AppSidebarService,
    private jWTTokenService: JWTTokenService
  ) {}
  ngOnInit(): void {}
  logout() {
    this.jWTTokenService.logout();
  }
  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }
}
