import { Component, OnInit } from '@angular/core';
import { IonHeader, IonToolbar, IonTitle, IonContent, IonSearchbar, IonCard, IonCardHeader, IonCardTitle, IonCardSubtitle, IonCardContent, IonInput, IonItem, IonButton, IonButtons, IonModal, IonList, IonLabel, IonText, IonInfiniteScroll, IonRouterLink, IonNav, IonIcon, IonAvatar, IonChip, IonRow, IonGrid, IonCol } from '@ionic/angular/standalone';
import { PostService } from '../services/post-service';
import { PostResponse } from '../models/postResponse';
import { Comment } from '../models/comment';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommentRequest } from '../models/commentRequest';
import { RouterLinkActive, RouterModule } from "@angular/router";
import { AuthenticatePage } from '../authenticate/authenticate.page';
import { RouterLink } from '@angular/router';
import { Endpoints } from '../helpers/endpoints';
import { ThemeService } from '../services/theme-service';
import { Share } from '@capacitor/share';
import { AuthService } from '../services/auth-service';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
  imports: [IonCol, IonGrid, IonRow, IonChip, IonAvatar, IonIcon, RouterModule, IonNav, IonText, IonLabel, IonList, IonModal, IonButtons, IonButton, IonItem, IonInput, IonCardContent, IonCardSubtitle,
    IonCardTitle, IonCardHeader, IonCard, IonSearchbar, IonHeader, IonToolbar, IonTitle, IonContent, IonInfiniteScroll, FormsModule, RouterLinkActive, IonRouterLink],
})
export class HomePage {
  private loggedInUsername: string | null = ""


  constructor(private postService: PostService,
    private httpclient: HttpClient,
    private authService: AuthService
  ) { }
  AuthenticateRoute = AuthenticatePage;
  themeService: ThemeService = ThemeService.getInstance();
  searchQuery: string = ""
  initialPosts: PostResponse[] = []
  filteredPosts: PostResponse[] = []
  comment: CommentRequest = new (CommentRequest)

  async ionViewWillEnter() {
    this.postService.GetAllPosts().subscribe(async response => {
      this.initialPosts = response;
      this.filteredPosts = response;
      this.loggedInUsername = await this.authService.getUsername();
    });
  }

  onSearch(): void {
    if (!this.searchQuery.trim()) {
      this.filteredPosts = this.initialPosts;
    } else {
      const query = this.searchQuery.toLowerCase();
      this.filteredPosts = this.initialPosts.filter(post =>
        post.title.toLowerCase().includes(query) ||
        post.content.toLowerCase().includes(query) ||
        post.username.toLowerCase().includes(query)
      );
    }
  }

  selectedPost: PostResponse | null = null;

  openPost(post: PostResponse) {
    this.selectedPost = post;
  }

  submitComment() {
    if (this.comment.content === null || this.comment.content.trim() === "")
      return;

    if (this.selectedPost?.id !== undefined)
      this.comment.postId = this.selectedPost?.id;

    console.log(this.comment);

    this.httpclient.post<Comment>(Endpoints.REQUEST_COMMENT, this.comment, { withCredentials: true }).subscribe(
      (newComment: Comment) => {
        if (this.selectedPost) {
          if (!this.selectedPost.comments) {
            this.selectedPost.comments = [];
          }
          this.selectedPost.comments.push(newComment);

          const postIndex = this.initialPosts.findIndex(p => p.id === this.selectedPost?.id);
          if (postIndex !== -1) {
            this.initialPosts[postIndex].comments = this.selectedPost.comments;
          }
        }

        this.comment.content = "";
        this.comment.postId = 0;
      },
      (error) => {
        console.error('Error submitting comment:', error);
      }
    );
  }

  closePost() {
    this.selectedPost = null;
  }

  toggleTheme() {
    this.themeService.toggleTheme()
  }

  isPostOwner(post: PostResponse): boolean {
    return this.loggedInUsername === post.username;
  }

  deletePost(post: PostResponse): void {
    console.log(post.id)
    this.postService.DeletePost(post.id).subscribe({
    
      error: (err) => this.ionViewWillEnter
    });
    this.ionViewWillEnter()
  }
  
}
