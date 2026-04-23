import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonContent, IonHeader, IonTitle, IonToolbar, IonLabel, IonInput, IonButton, IonButtons, IonIcon } from '@ionic/angular/standalone';
import { HttpClient } from '@angular/common/http';
import { Post } from '../models/post';
import { Endpoints } from '../helpers/endpoints';
import { RouterLink, RouterLinkActive } from '@angular/router';
@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.page.html',
  styleUrls: ['./create-post.page.scss'],
  standalone: true,
  imports: [IonInput, IonLabel, IonContent, IonHeader, IonTitle, IonToolbar, CommonModule, FormsModule, IonButton, IonButtons, IonIcon, RouterLink, RouterLinkActive]
})
export class CreatePostPage {

  post :Post = new Post(); 
  constructor(private httpClient :HttpClient) { }

  ngOnInit() {
  }

  async submit() {
    console.log(this.post);
    this.httpClient.post(Endpoints.REQUEST_POST, this.post, {withCredentials: true}).subscribe();
  }

}
