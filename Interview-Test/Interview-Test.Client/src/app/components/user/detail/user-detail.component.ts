import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { UserModel } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
    standalone: true,
    selector: 'app-user-detail',
    imports: [CommonModule],
    templateUrl: './user-detail.component.html',
})
export class UserDetailComponent {
    private userService = inject(UserService);

    userId!: string | null;
    user = signal<UserModel | null>(null);
    constructor(private route: ActivatedRoute, private router: Router) { }

    ngOnInit(): void {
        this.route.paramMap.subscribe((params: ParamMap) => {
            this.userId = params.get('id');
            this.loadUsers(this.userId);
        });

    }

    loadUsers(id: string | null) {
        this.userService.getUserById(id).subscribe(data => {
            this.user.set(data);
        });
    }

    routeMain() {
        this.router.navigate(['/users']);
    }
}
