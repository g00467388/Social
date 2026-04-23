const baseUrl = "http://localhost:5188"

export const enum Endpoints 
{
    PAGE_AUTH  = '/authenticate',
    PAGE_HOME = '/home',
    PAGE_REQUEST = '/comment',
    PAGE_CREATE_POST  = '/create-post',

    REQUEST_LOGIN = `${baseUrl}/auth/login`,
    REQUEST_REGISTER = `${baseUrl}/auth/register`,
    REQUEST_POST = `${baseUrl}/post`,
    REQUEST_POST_LIST = `${baseUrl}/post/all`,
    REQUEST_POST_BY_TITLE = `${baseUrl}/post/title`,
    REQUEST_COMMENT = `${baseUrl}/comment`,
    
}
