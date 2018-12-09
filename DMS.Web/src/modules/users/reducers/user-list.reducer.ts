import { UserList } from '../types/store';
import { UserListActions } from '../actions/user-list.actions';
import * as constants from "../constants/user-list.constants";

export default function userList(state: UserList = {}, action: UserListActions) {
    switch(action.type) {
        case constants.LOAD_USER_LIST: {
            return {...state, users: action.users}
        }
    }

    return state;
}