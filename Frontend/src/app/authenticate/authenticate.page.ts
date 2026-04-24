import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonContent, IonHeader, IonTitle, IonToolbar, IonInput, IonText, IonLabel, IonButton, ToastController, LoadingController, IonButtons, IonIcon } from '@ionic/angular/standalone';
import { Auth } from '../models/auth';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { ThemeService } from '../services/theme-service';

@Component({
  selector: 'app-authenticate',
  templateUrl: './authenticate.page.html',
  styleUrls: ['./authenticate.page.scss'],
  standalone: true,
  imports: [
    IonLabel, IonText, IonInput, IonContent, IonHeader, IonTitle, IonToolbar,
    CommonModule, FormsModule, IonButton, IonButtons, IonIcon, RouterLink, RouterLinkActive
  ]
})
export class AuthenticatePage {
  themeService : ThemeService = ThemeService.getInstance();

  auth: Auth = new Auth();
  isSignupPage: boolean = true;

  constructor(
    private authService : AuthService
  ) {}

  SwapAuthPage() {
    this.isSignupPage = !this.isSignupPage;
  }

  async submit() {
    if (this.isSignupPage)
      this.authService.submitSignup(this.auth);
    else
      this.authService.submitLogin(this.auth);
  }

  toggleTheme() {
    this.themeService.toggleTheme()
  }

}
