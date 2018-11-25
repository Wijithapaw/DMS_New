import { Dashboard } from "modules/dashboard/components";
import { UserManagement, UserDetails } from "modules/users/components";

var dashRoutes = [
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
