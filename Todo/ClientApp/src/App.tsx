import { Component } from "react";
import { Route, Routes } from "react-router-dom";
import AppRoutes from "./AppRoutes";
import "./custom.css";
import { FluentProvider, webLightTheme } from "@fluentui/react-components";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <FluentProvider theme={webLightTheme}>
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Routes>
      </FluentProvider>
    );
  }
}
