import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonContent, IonHeader, IonTitle, IonToolbar, IonLabel, IonInput, IonButton } from '@ionic/angular/standalone';
import { HttpClient } from '@angular/common/http';
import { Post } from '../models/post';
import { Endpoints } from '../helpers/endpoints';
@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.page.html',
  styleUrls: ['./create-post.page.scss'],
  standalone: true,
  imports: [IonInput, IonLabel, IonContent, IonHeader, IonTitle, IonToolbar, CommonModule, FormsModule, IonButton]
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
