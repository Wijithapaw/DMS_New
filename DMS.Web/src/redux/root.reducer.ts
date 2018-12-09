import { combineReducers } from "redux";
import { userReducers } from "../modules/users/reducers";
import { projectsReducers } from "../modules/projects/reducers";

export const rootReducer = combineReducers({
    ...userReducers,
    ...projectsReducers
})