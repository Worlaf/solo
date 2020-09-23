import React from "react";
import "./App.css";
import ParkList from "./pages/ParkList";
import MainLayout from "./layouts/MainLayout";
import { BrowserRouter, Redirect, Route, Switch } from "react-router-dom";
import { routes } from "./routes/routes";
import ParkDetails from "./pages/ParkDetails";

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <MainLayout>
          <Switch>
            <Route exact path={routes.parkList} component={ParkList} />
            <Route path={routes.parkDetails} component={ParkDetails} />
            <Route>
              <Redirect to={routes.parkList} />
            </Route>
          </Switch>
        </MainLayout>
      </BrowserRouter>
    </div>
  );
}

export default App;
