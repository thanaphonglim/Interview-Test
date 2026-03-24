import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { UserModel } from 'src/app/models/user.model';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-users-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './users-list.component.html',
})
export class UsersListComponent {

  private userService = inject(UserService);
  users = signal<UserModel[]>([]);
  searchText = signal('');

  filteredUsers = computed(() => {
    const keyword = this.searchText().toLowerCase();

    return this.users().filter(u =>
      u.id?.toString().toLowerCase().includes(keyword) ||
      u.userId?.toLowerCase().includes(keyword) ||
      u.username?.toLowerCase().includes(keyword) ||
      `${u.firstName} ${u.lastName}`.toLowerCase().includes(keyword)
    );
  });

  constructor(private router: Router) {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers().subscribe(data => {
      this.users.set(data);
    });
  }

  route(id: string) {
    this.router.navigate(['/users', id]);
  }


}
