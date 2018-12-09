import * as constants from '../constants/user-list.constants';
import { User } from '../types/store';
import { Dispatch } from 'redux';
import { userService } from '../services/user.service';

export const userActions = {
    loadUserList
}

export interface LoadUserList {
    type: constants.LOAD_USER_LIST;
    users: User[];
}

export type UserListActions = LoadUserList;

function loadUserList() {
    return (dispatch: Dispatch<UserListActions>) => {
        userService.getAll()
            .then((users: User[]) => {
                dispatch({ type: constants.LOAD_USER_LIST, users });
            });
    }
}