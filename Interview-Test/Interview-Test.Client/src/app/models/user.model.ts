export interface UserModel {
  id: string;
  userId: string;
  username: string;
  firstName: string;
  lastName: string;
  age: number | null;
  roles: RoleModel[];
  permissions: string[];
}

export interface RoleModel {
  roleId: number;
  roleName: string;
}

interface PermissionModel {
  permissionId: string;
  permission: string;
}

interface UserListModel {
  id: string;
  userId: string;
  username: string;
  firstName: string;
  lastName: string;
  age?: number;
  roles: RoleModel[];
  permissions: PermissionModel[];
}