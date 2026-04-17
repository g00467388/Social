import { Component, OnInit } from '@angular/core';
import { IonHeader, IonToolbar, IonTitle, IonContent, IonSearchbar } from '@ionic/angular/standalone';
import { PostService } from '../services/post-service';
@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
  imports: [IonSearchbar, IonHeader, IonToolbar, IonTitle, IonContent],
})
export class HomePage implements OnInit {
  constructor(private postService : PostService) {}
  // bound to search bar
  searchQuery :string = ""

  ngOnInit(): void {
    this.postService.GetAllPosts();
  }

  async search() : Promise<void> {
    (await this.postService.GetPostByTitle(this.searchQuery)).forEach(x => console.log(x));
  }
}
