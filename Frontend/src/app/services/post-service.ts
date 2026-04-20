import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '../models/post';
import { Observable } from 'rxjs';
import { PostResponse } from '../models/postResponse';

@Injectable({
  providedIn: 'root',
})
export class PostService {
  BaseUrl: string = "http://localhost:5188"
  constructor(private httpClient: HttpClient) { }

 GetAllPosts(): Observable<PostResponse[]> {
  return this.httpClient.get<PostResponse[]>(this.BaseUrl + "/post/all");
}
  
    async GetPostByTitle(title :string): Promise<Observable<Post[]>> {

    let searchParams : HttpParams = new HttpParams()
    searchParams.set("Title: ", title)

    return await this.httpClient.get<Post[]>(this.BaseUrl, {params: searchParams} );
  }

}
