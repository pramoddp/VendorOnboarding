import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {
    // tslint:disable-next-line:max-line-length
    MatButtonModule, MatCardModule, MatFormFieldModule, MatIconModule, MatInputModule, MatSelectModule,
    MatStepperModule, MatListModule, MatMenuModule, MatRadioModule, MatSidenavModule, MatToolbarModule,
    MatProgressSpinnerModule, MatTooltipModule, MatDatepickerModule
} from '@angular/material';

import { FuseSharedModule } from '@fuse/shared.module';
import { FileUploadModule } from 'ng2-file-upload';
import { MenuAppComponent } from './menu-app/menu-app.component';
import { RoleComponent } from './role/role.component';
import { UserComponent } from './user/user.component';
import { ReasonComponent } from './reason/reason.component';
import { IdentityComponent } from './identity/identity.component';
import { BankComponent } from './bank/bank.component';
import { OnBoardingFieldMasterComponent } from './on-boarding-field-master/on-boarding-field-master.component';

const menuRoutes: Routes = [
    {
        path: 'menuApp',
        component: MenuAppComponent,
    },
    {
        path: 'role',
        component: RoleComponent,
    },
    {
        path: 'user',
        component: UserComponent,
    },
    {
        path: 'reason',
        component: ReasonComponent,
    },
    {
        path: 'identity',
        component: IdentityComponent,
    },
    {
        path: 'bank',
        component: BankComponent,
    },
    {
        path: 'obdfield',
        component: OnBoardingFieldMasterComponent,
    },
];
@NgModule({
    declarations: [
        UserComponent,
        RoleComponent,
        MenuAppComponent,
        ReasonComponent,
        IdentityComponent,
        BankComponent,
        OnBoardingFieldMasterComponent
    ],
    imports: [
        MatButtonModule,
        MatCardModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatSelectModule,
        MatStepperModule,
        MatProgressSpinnerModule,
        MatListModule,
        MatMenuModule,
        MatRadioModule,
        MatSidenavModule,
        MatToolbarModule,
        MatTooltipModule,
        FuseSharedModule,
        FileUploadModule,
        MatDatepickerModule,
        RouterModule.forChild(menuRoutes)
    ],
    providers: [

    ]
})
export class MasterModule {
}

