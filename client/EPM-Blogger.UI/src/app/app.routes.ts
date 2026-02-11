import { Routes } from '@angular/router';
import { RegisterComponent } from './authentication/register/register.component';
import { LoginComponent } from './authentication/login/login.component';
import { AppComponent } from './app.component';
import { PostListComponent } from './Posts/post-list/post-list.component';
import { ProfileComponent } from './General/profile/profile.component';
import { CreateComponent } from './Posts/create/create.component';

export const routes: Routes = [
    {
        path: 'register',
        component: RegisterComponent,
        title: "Register your Account"
    },
    {
        path: 'login',
        component: LoginComponent,
        title: 'Login to your Account'
    },
    {
        path: '',
        redirectTo: '/posts',
        pathMatch: 'full'
    },
    {
        path: 'posts',
        component: PostListComponent,
        title: "Posts Component"
    },
    {
        path:'user',
        component:ProfileComponent,
        title:"User Profile"
    },
    {
        path:'posts/new',
        component:CreateComponent,
        title:"Create New Post"
    }

];
