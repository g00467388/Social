import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '../models/post';
import { Observable } from 'rxjs';
import { PostResponse } from '../models/postResponse';
import { Endpoints } from '../helpers/endpoints';

@Injectable({
  providedIn: 'root',
})
export class PostService {
  constructor(private httpClient: HttpClient) { }

  GetAllPosts(): Observable<PostResponse[]> {
    return this.httpClient.get<PostResponse[]>(Endpoints.REQUEST_POST_LIST);
  }
  GetPostByTitle(title: string): Observable<PostResponse[]> {
    console.log("service title value: " + title)
    return this.httpClient.get<PostResponse[]>(Endpoints.REQUEST_POST_BY_TITLE + "?title=" + title);
  }
  DeletePost(id: number) {
   return this.httpClient.delete(`${Endpoints.REQUEST_POST_DELETE}/${id}`,{ withCredentials: true });

  }


}

