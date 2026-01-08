import React from "react";
import { Link, useLocation } from "react-router-dom";

import { NavItem } from "../../types/NavItem";
import styles from "./MobileNavBar.module.css";

interface MobileNavBarProps {
  navItems: NavItem[];
}

const MobileNavBar: React.FC<MobileNavBarProps> = ({ navItems }) => {
  const location = useLocation();

  const bgMap: Record<string, string> = {
    "/": "stadium-high.jpg",
    "/about": "stadium-high.jpg",
    "/letterbox": "danke-schoen.jpg",
    "/blog": "crazy.jpg",
    "/operation-kino": "pontiac.jpg",
    "/box-office": "amanda.jpg",
  };

  return (
    <nav
      className={`${styles.mobileNavBar} bg-dark`}
      style={{
        backgroundImage: `url(https://cdn.allenmaygibson.com/images/static/${
          bgMap["/" + location.pathname.split("/")[1]]
        })`,
      }}
    >
      <nav className={styles.navItems}>
        {navItems.map((item) =>
          "/" + location.pathname.split("/")[1] === item.path ? (
            <h3 key={item.path} className={styles.navItemSelected}>
              {item.label}
            </h3>
          ) : (
            <span key={item.path} className={styles.navItem}>
              <Link className={styles.navItem} to={item.path}>
                {item.label}
              </Link>
            </span>
          )
        )}
      </nav>
    </nav>
  );
};

export default MobileNavBar;
