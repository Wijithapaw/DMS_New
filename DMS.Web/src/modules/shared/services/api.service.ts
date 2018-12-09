import axios from "axios";

axios.defaults.baseURL = "http://localhost:6288/api";

export const apiService = {
    get
}

function get<T>(controller: string, action: string = '', urlParams: string[] = [], queryParams: any = null) {
    var url = createUrl(controller, action, urlParams, queryParams);
    return axios.get<T>(url).then(res => res.data);
}

function createUrl(controller: string, action: string = '', urlParams: string[] = [], queryParams: any = null) {
    let url = controller + (action ? '/' + action : '');

    urlParams.forEach(param => {
        url += '/' + param;
    });
  
    if(queryParams) {
        let queryParamsArr = Object.keys(queryParams)    
        let queryParamStr = '';
        queryParamsArr.forEach(param => {
            queryParamStr +=  param + "&";
        });
        url += "?" + queryParamStr.slice(0, -1);
    }

    return url;
}