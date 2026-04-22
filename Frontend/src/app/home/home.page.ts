import { Component, OnInit } from '@angular/core';
import { IonHeader, IonToolbar, IonTitle, IonContent, IonSearchbar, IonCard, IonCardHeader, IonCardTitle, IonCardSubtitle, IonCardContent, IonInput, IonItem, IonButton, IonButtons, IonModal, IonList, IonLabel, IonText, IonInfiniteScroll } from '@ionic/angular/standalone';
import { PostService } from '../services/post-service';
import { PostResponse } from '../models/postResponse';
import { Comment } from '../models/comment';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommentRequest } from '../models/commentRequest';
@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
  imports: [IonText, IonLabel, IonList, IonModal, IonButtons, IonButton, IonItem, IonInput, IonCardContent, IonCardSubtitle, 
    IonCardTitle, IonCardHeader, IonCard, IonSearchbar, IonHeader, IonToolbar, IonTitle, IonContent, IonInfiniteScroll, FormsModule],
})
export class HomePage implements OnInit {
  constructor(private postService : PostService, private httpclient : HttpClient) {}
  searchQuery :string = ""
  initialPosts :PostResponse[] = []
  comment : CommentRequest = new(CommentRequest)


  ngOnInit(): void {
    this.postService.GetAllPosts().subscribe(response => 
      this.initialPosts = response
    );
    
  }


selectedPost: PostResponse | null = null;

openPost(post: PostResponse) {
  this.selectedPost = post;
}

submitComment() {
  if (this.comment === null)
    return; 
  if (this.selectedPost?.id !== undefined)
    this.comment.postId = this.selectedPost?.id
  console.log(this.comment);
  this.httpclient.post<CommentRequest>("http://localhost:5188/comment", this.comment, {withCredentials: true} ).subscribe();
  
  this.comment.content = ""
  this.comment.postId = 0
}
closePost() {
  this.selectedPost = null;
}
  async search() : Promise<void> {
    (await this.postService.GetPostByTitle(this.searchQuery)).forEach(x => console.log(x));
  }
}
