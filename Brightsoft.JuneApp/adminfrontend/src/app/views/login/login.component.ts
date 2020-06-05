import { JWTTokenService } from "./../../jwt-token.service";
import { Component } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { UserService } from "../../user.service";
import { Router } from '@angular/router';

@Component({
  selector: "app-dashboard",
  templateUrl: "login.component.html",
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private jWTTokenService: JWTTokenService,
    private router: Router
    ) {
    this.loginForm = this.formBuilder.group({
      id: "",
      password: "",
    });
  }
  onLogin() {
    this.userService.login(this.loginForm.value).subscribe((result) => {
      if (!result.errors) {
        this.jWTTokenService.setToken(result.data.login.accessToken);
        this.router.navigate(["/dashboard"]).catch(console.log);
      } else {
        console.error("ERROR in login", result.errors);
      }
    });
  }
}
