import * as React from "react";
// javascript plugin used to create scrollbars on windows
import PerfectScrollbar from "perfect-scrollbar";
import { Route, Switch, Redirect } from "react-router-dom";

import { Sidebar, Header, Footer } from "src/layouts/components";
import authenticatedRoutes from "src/routes/authenticated";

import { PageHeader } from "src/layouts/components";
import { Row, Col } from 'reactstrap';

var ps: PerfectScrollbar;

class FullLayout extends React.Component<any> {

  constructor(props: any) {
    super(props);

    this.refMaiPanel = React.createRef();
  }

  private refMaiPanel: React.RefObject<HTMLDivElement>;

  componentDidMount() {
    if (navigator.platform.indexOf("Win") > -1 && this.refMaiPanel.current) {
      ps = new PerfectScrollbar(this.refMaiPanel.current);
      document.body.classList.toggle("perfect-scrollbar-on");
    }
  }
  componentWillUnmount() {
    if (navigator.platform.indexOf("Win") > -1) {
      ps.destroy();
      document.body.classList.toggle("perfect-scrollbar-on");
    }
  }
  componentDidUpdate(e: any) {
    if (e.history.action === "PUSH") {
      if (this.refMaiPanel.current)
        this.refMaiPanel.current.scrollTop = 0;

      if (document.scrollingElement)
        document.scrollingElement.scrollTop = 0;
    }
  }
  render() {
    return (
      <div className="wrapper">
        <Sidebar {...this.props} routes={authenticatedRoutes} />
        <div className="main-panel" ref={this.refMaiPanel}>
          <Header {...this.props} />
          <PageHeader size="sm" />
          <Switch>
            <div>
              <div className="content">
                <Row>
                  <Col xs={12}>
                    {authenticatedRoutes.map((prop, key) => {
                      if (prop.collapse && prop.views) {
                        return prop.views.map((prop2, key2) => {
                          return (
                            <Route
                              path={prop2.path}
                              component={prop2.component}
                              key={key2}
                            />
                          );
                        });
                      }
                      if (prop.redirect && prop.pathTo)
                        return <Redirect from={prop.path} to={prop.pathTo} key={key} />;
                      return (
                        <Route path={prop.path} component={prop.component} key={key} />
                      );
                    })}
                  </Col> 
                </Row>
              </div>
            </div>
          </Switch>
          <Footer fluid />
        </div>
      </div>
    );
  }
}

export default FullLayout;
