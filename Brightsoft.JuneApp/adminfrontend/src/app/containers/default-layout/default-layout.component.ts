import { AppCookieService } from "./../../cookie.service";
import { Component } from "@angular/core";
import { navItems } from "../../_nav";

@Component({
  selector: "app-dashboard",
  templateUrl: "./default-layout.component.html",
})
export class DefaultLayoutComponent {
  public sidebarMinimized = false;
  public navItems = navItems;
  constructor(private appCookieService: AppCookieService) {}
  logout() {
    this.appCookieService.remove("userToken");

  }
  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }
}
