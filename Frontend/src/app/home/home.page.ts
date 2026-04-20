import { Component, OnInit } from '@angular/core';
import { IonHeader, IonToolbar, IonTitle, IonContent, IonSearchbar, IonCard, IonCardHeader, IonCardTitle, IonCardSubtitle, IonCardContent, IonInput, IonItem, IonButton, IonButtons, IonModal, IonList, IonLabel, IonText } from '@ionic/angular/standalone';
import { PostService } from '../services/post-service';
import { PostResponse } from '../models/postResponse';
@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
  imports: [IonText, IonLabel, IonList, IonModal, IonButtons, IonButton, IonItem, IonInput, IonCardContent, IonCardSubtitle, IonCardTitle, IonCardHeader, IonCard, IonSearchbar, IonHeader, IonToolbar, IonTitle, IonContent],
})
export class HomePage implements OnInit {
  constructor(private postService : PostService) {}
  // bound to search bar
  searchQuery :string = ""
  initialPosts :PostResponse[] = []

  ngOnInit(): void {
    this.postService.GetAllPosts().subscribe(response => 
      this.initialPosts = response
    );
    
  }


selectedPost: PostResponse | null = null;

openPost(post: PostResponse) {
  this.selectedPost = post;
}

closePost() {
  this.selectedPost = null;
}
  async search() : Promise<void> {
    (await this.postService.GetPostByTitle(this.searchQuery)).forEach(x => console.log(x));
  }
}
