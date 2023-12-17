import DashboardLayout from '../layout/DashboardLayout.vue'
// GeneralViews
import NotFound from '../pages/NotFoundPage.vue'

// Admin pages
import Overview from 'src/pages/Overview.vue'
import UserProfile from 'src/pages/UserProfile.vue'
import TableList from 'src/pages/TableList.vue'
import Typography from 'src/pages/Typography.vue'
import Icons from 'src/pages/Icons.vue'
import Maps from 'src/pages/Maps.vue'
import Notifications from 'src/pages/Notifications.vue'
import Upgrade from 'src/pages/Upgrade.vue'
import Event from 'src/pages/Event/Event.vue'
import EventDetail from 'src/pages/Event/Detail/EventDetail.vue'
import CreateEvent from 'src/pages/Event/Detail/CreateEvent.vue'
import CreateEventDetail from 'src/pages/Event/Detail/CreateEventDetail.vue'
import Login from 'src/pages/Login.vue'

const routes = [
  {
    name: 'Login',
    component: Login,
    path: '/login'
  },
  {
    path: '/',
    component: DashboardLayout,
    redirect: '/admin/overview'
  },
  {
    path: '/admin',
    component: DashboardLayout,
    redirect: '/admin/overview',
    children: [
      {
        path: 'overview',
        name: 'Overview',
        component: Overview
      },
      {
        path: 'user',
        name: 'User',
        component: UserProfile
      },
      {
        path: 'table-list',
        name: 'Table List',
        component: TableList
      },
      {
        path: 'typography',
        name: 'Typography',
        component: Typography
      },
      {
        path: 'icons',
        name: 'Icons',
        component: Icons
      },
      {
        path: 'maps',
        name: 'Maps',
        component: Maps
      },
      {
        path: 'notifications',
        name: 'Notifications',
        component: Notifications
      },
      {
        path: 'upgrade',
        name: 'Upgrade to PRO',
        component: Upgrade
      },
      {
        path: 'event',
        name: 'Event',
        component: Event,
        // children: [
        //   {

        // ]
      },
      {
        path: 'event/detail',
        name: 'EventDetail',
        component: EventDetail
      },
      {
        path: 'event/create',
        name: 'CreateEvent',
        component: CreateEvent
      },
      {
        path: 'event/detail/create',
        name: 'CreateEventDetail',
        component: CreateEventDetail
      }
    ]
  },
  { path: '*', component: NotFound }
]

/**
 * Asynchronously load view (Webpack Lazy loading compatible)
 * The specified component must be inside the Views folder
 * @param  {string} name  the filename (basename) of the view to load.
function view(name) {
   var res= require('../components/Dashboard/Views/' + name + '.vue');
   return res;
};**/

export default routes
