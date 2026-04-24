import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LoadingController, ToastController } from '@ionic/angular';
import { Auth } from '../models/auth';
import { Endpoints } from '../helpers/endpoints';
import { Preferences } from '@capacitor/preferences';

@Injectable({
  providedIn: 'root',
})

export class AuthService {
  constructor(
    private httpclient: HttpClient,
    private toastCtrl: ToastController,
    private loadingCtrl: LoadingController,
    private router: Router,
  ) { }

  async submitLogin(authRequest: Auth) {
    await this.setUsername(authRequest.username);
    
    const loading = await this.loadingCtrl.create({ message: 'Logging in...' });
    await loading.present();

    this.httpclient.post(Endpoints.REQUEST_LOGIN, authRequest, { withCredentials: true, observe: 'response' })
      .subscribe({
        next: async () => {
          await loading.dismiss();
          this.showToast("Login successful!");
          this.router.navigate(['/home']);
        },
        error: async () => {
          await loading.dismiss();
          this.showToast("Login failed. Invalid username or password :(");
        }
      });
  }

  async submitSignup(authRequest: Auth) {
    const loading = await this.loadingCtrl.create({ message: 'Creating account...' });
    await loading.present();

    this.httpclient.post(Endpoints.REQUEST_REGISTER, authRequest, { withCredentials: true })
      .subscribe({
        next: async () => {
          await loading.dismiss();
          this.showToast("Account created! You can now log in.");
        },
        error: async () => {
          await loading.dismiss();
          this.showToast("Signup failed. Try again.");
        }
      });
  }

  private async showToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      duration: 2000,
      position: 'bottom',
      color: 'primary'
    });
    toast.present();
  }

  async setUsername(username: string) {
    Preferences.set({
      key: 'username',
      value: username
    });
  }
async getUsername() {
  let username = Preferences.get({ key: 'username' });
  console.log((await username).value)
  return (await username).value 
}
}
