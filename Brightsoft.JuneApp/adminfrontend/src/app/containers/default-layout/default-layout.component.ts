import { Component } from "@angular/core";
import { navItems } from "../../_nav";

@Component({
  selector: "app-dashboard",
  templateUrl: "./default-layout.component.html",
})
export class DefaultLayoutComponent {
  public sidebarMinimized = false;
  public navItems = navItems;
  constructor() {}
  logout() {
    localStorage.removeItem("userToken");

  }
  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }
}
