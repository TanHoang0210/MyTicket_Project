import moment from 'moment';
import numeral from 'numeral';

const helpService = {
  formatDate: (date) => {
    return moment(date).format('hh:mm DD/MM/YYYY');
  },
  formatDay: (date) => {
    return moment(date).format('DD/MM/YYYY');
  },
  formatCurrency: (value) => {
    return numeral(value).format('0,0') + ' VND';
  }
};

export default helpService;