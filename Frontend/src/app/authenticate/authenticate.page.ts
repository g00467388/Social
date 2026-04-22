import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonContent, IonHeader, IonTitle, IonToolbar, IonInput, IonText, IonLabel, IonButton } from '@ionic/angular/standalone';
import { Auth } from '../models/auth';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-authenticate',
  templateUrl: './authenticate.page.html',
  styleUrls: ['./authenticate.page.scss'],
  standalone: true,
  imports: [IonLabel, IonText, IonInput, IonContent, IonHeader, IonTitle, IonToolbar, CommonModule, FormsModule, IonButton]
})
export class AuthenticatePage implements OnInit {
  auth :Auth = new(Auth);
  isSignInPage :boolean = true;


  constructor(private httpclient :HttpClient) { 
    
  }
  
  ngOnInit() { 

  }

  SwapAuthPage()
  {
    this.isSignInPage = !this.isSignInPage;
  }

  async submit() {
    if (this.isSignInPage)
      this.submitSignup(); 
    else 
      this.submitLogin();
  }

  private async submitLogin() {
    this.httpclient.post("http://localhost:5188/auth/login", this.auth, {withCredentials: true}).subscribe(response => {
      console.log(response);
    })
  }

  private async submitSignup() {
    this.httpclient.post("http://localhost:5188/auth/register", this.auth, {withCredentials: true, observe: 'response'}).subscribe(response => {
      console.log(response);
    })
  }
}
