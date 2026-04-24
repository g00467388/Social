# Capacitor usage  
## Auth Service
Capacitor usage can be located in the both the services/auth-service.ts file and services/theme-service.ts, The intended purpose of it's implementation is to store the username of the user upon logging in. When navigating to the main page, this is used to determine whether or not a post belongs to the currently logged in user, if the post does belong to the currently logged in user, a delete button will appear 

## Theme service 
Theme service is a singleton class used to modify the theme the program is currently using, it saves the current theme in preferences and then applies it upon each click of the "Change theme" button 

* HttpClient usage 
HttpClient usage is ubiquitous in this program, it is used primarily in *services/auth-service.ts* and *services/post-service.ts* 

