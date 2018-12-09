import { Dashboard } from "src/modules/dashboard/components";
import { UserDetails } from "src/modules/users/components";
import { UserManagementContainer } from '../modules/users/containers';
import { ProjectsManagementContainer } from '../modules/projects/containers';

export interface RouteNode {
  path: string;
  pathTo?: string;
  name?: string;
  icon?: string;
  component?: any;
  collapse?: boolean;
  redirect?: boolean;
  views?: RouteNode[];
  pro?: boolean;
}

var dashRoutes: RouteNode[] = [
  {
    path: "/dashboard",
    name: "Dashboard",
    icon: "design_app",
    component: Dashboard
  },
  {
    path: "/user-details",
    name: "User Details",
    icon: "users_single-02",
    component: UserDetails
  },
  {
    path: "/user-management",
    name: "Users",
    icon: "files_paper",
    component: UserManagementContainer
  },
  {
    path: "/projects-management",
    name: "Projects",
    icon: "files_paper",
    component: ProjectsManagementContainer
  },
  { redirect: true, path: "/", pathTo: "/dashboard", name: "Dashboard" }
];
export default dashRoutes;

