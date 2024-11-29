import { Outlet } from "react-router-dom";

import styles from "./BlogLayout.module.css";

function BlogLayout() {
  return (
    <>
      <div className={styles.blogLayout}>
        <Outlet />
      </div>
    </>
  );
}

export default BlogLayout;
