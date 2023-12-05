// apiService.js
import axios from 'axios';
import store from '@/store';
const authService = axios.create({
    baseURL: 'connect/token',
});
export const refreshAccessToken = async (refreshTokenNow) => {
    const xx =5;
    if (xx == 5) {
        try {
            const requestConfig = {
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                paramsSerializer: params => {
                    return new URLSearchParams(params).toString();
                },
            };
            axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
            const res = await axios.post('connect/token',
                {
                    grant_type: 'refresh_token',
                    username: "hello",
                    password: "hello",
                    scope: 'offline_access',
                    client_id: 'client-angular',
                    client_secret: '52F4A9A45C1F21B53B62F56DA52F7',
                    refresh_token: refreshTokenNow
                }, requestConfig);
            store.commit('setTokens', {
                accessToken: res.data.access_token,
                refreshToken: res.data.refresh_token,
                tokenExpiration: res.data.expires_in,
            });
        } catch (error) {
            this.$toasted.error(error, {
                position: 'top-right',
                duration: 3000, // Thời gian hiển thị toast (ms)
            });
        }
    } else {
        await axios.post('connect/logout', {}, {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            }
        })
        store.dispatch('logout');
        alert("Phiên hết hạn")
    }
};

export const Login = async (username, password) => {
    const requestConfig = {
        withCredentials: true,
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        paramsSerializer: params => {
            return new URLSearchParams(params).toString();
        },
    };
    axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
    try {
        const res = await axios.post('connect/token',
            {
                grant_type: 'password',
                username: username,
                password: password,
                scope: 'offline_access',
                client_id: 'client-angular',
                client_secret: '52F4A9A45C1F21B53B62F56DA52F7',
            }, requestConfig);
        store.commit('setTokens', {
            accessToken: res.data.access_token,
            refreshToken: res.data.refresh_token,
            tokenExpiration: res.data.expires_in,
        });
        return res;
    } catch (error) {
        return error;
    }

};
export const getCurrentUser = async () => {
    try {
        const user = await axios.get(
            "myticket/api/user/current-user"
        )
        store.commit('setCurrentUser', user.data.data);
    } catch (error) {
        return error
    }
}
export default authService;