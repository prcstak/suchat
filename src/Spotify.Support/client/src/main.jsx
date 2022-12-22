import React, {createContext} from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import UserStore from "./stores/UserStore.js";

const userStore = new UserStore();
export const Context = createContext({
    userStore,
});

ReactDOM.createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <Context.Provider value={{userStore}}>
            <App/>
        </Context.Provider>
    </React.StrictMode>
)
