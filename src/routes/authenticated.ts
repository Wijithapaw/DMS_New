import { Dashboard } from "src/modules/dashboard/components";
import { UserManagement, UserDetails } from "src/modules/users/components";

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
    component: UserManagement
  },
  { redirect: true, path: "/", pathTo: "/dashboard", name: "Dashboard" }
];
export default dashRoutes;

