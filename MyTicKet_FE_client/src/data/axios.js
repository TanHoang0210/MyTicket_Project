import axios from "axios"

axios.defaults.baseURL = 'http://localhost:5030/'
axios.interceptors.request.use(
    config => {
      const token = localStorage.getItem('accessToken'); // Thay 'access_token' bằng tên token của bạn
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    error => {
      return Promise.reject(error);
    }
  );