import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { ProductsComponent } from './products/products.component';
import { ProductComponent } from './product/product.component';
import { ProductService } from './product.service';
import { AddProductComponent } from './add-product/add-product.component';
import { BasketComponent } from './basket/basket.component';
import { ProductInfoComponent } from './product-info/product-info.component';
import { ProductUpdateComponent } from './product-update/product-update.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ProductsComponent,
    ProductComponent,
    AddProductComponent,
    BasketComponent,
    ProductInfoComponent,
    ProductUpdateComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', redirectTo: '/products', pathMatch: 'full' },
      { path: 'products', component: ProductsComponent },
      { path: 'products/:id', component: ProductComponent },
      { path: 'add-product', component: AddProductComponent},
      { path: 'basket', component: BasketComponent },
    ])
  ],
  providers: [
    ProductService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }