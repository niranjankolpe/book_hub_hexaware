import { Routes } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { HomeComponent } from './components/home/home.component';
import { ContactUsComponent } from './components/contact-us/contact-us.component';
import { UserRegisterFormComponent } from './components/user-register-form/user-register-form.component';
import { LoginComponent } from './components/login/login.component';
import { ExploreBooksComponent } from './components/explore-books/explore-books.component';

export const routes: Routes = [
    {
        path:'',
        redirectTo: '/app-home',
        pathMatch: 'full'
    },
    {
        path: 'app-header',
        component: HeaderComponent
    },
    {
        path: 'app-footer',
        component: FooterComponent
    },
    {
        path: 'app-explore-books',
        component: ExploreBooksComponent
    },
    {
        path: 'app-home',
        component: HomeComponent
    },
    {
        path: 'app-user-register-form',
        component: UserRegisterFormComponent
    },
    {
        path: 'app-login',
        component: LoginComponent
    },
    {
        path: 'app-contact-us',
        component: ContactUsComponent
    }
];
