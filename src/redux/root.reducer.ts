import { combineReducers } from "redux";
import { userReducers } from "../modules/users/reducers";

export const rootReducer = combineReducers({
    ...userReducers
})