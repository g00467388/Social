import { comment } from "./comment";

export class PostResponse {
    id :number = 0;
    title : string = ""
    content :string = ""
    createdAt :Date = new Date
    userId : string = ""
    username : string = ""
    comments :comment[] = [];
}