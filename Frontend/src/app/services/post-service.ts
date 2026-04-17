import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '../models/post';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PostService {
  BaseUrl: string = "http://localhost:5146"
  constructor(private httpClient: HttpClient) { }

  async GetAllPosts(): Promise<Observable<Post[]>> {
    return await this.httpClient.get<Post[]>(this.BaseUrl + "/post/all");
  }
  
    async GetPostByTitle(title :string): Promise<Observable<Post[]>> {

    let searchParams : HttpParams = new HttpParams()
    searchParams.set("Title: ", title)

    return await this.httpClient.get<Post[]>(this.BaseUrl, {params: searchParams} );
  }

}
